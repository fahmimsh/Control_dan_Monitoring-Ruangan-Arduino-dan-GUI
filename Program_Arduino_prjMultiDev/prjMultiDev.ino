/*Include Library*/
#include <SoftwareSerial.h>

/*Definisi Mapping-IO*/
#define   btnPanic  3
#define   addrJP0   6
#define   addrJP1   5
#define   inRL1     7
#define   inRL2     8
#define   otRL1     9
#define   otRL2     10
#define   otBuz     11  
#define   aSuhu1    A0
#define   aSuhu2    A1 

//for Serial LCD
#define   lcdCmd    254
#define   lcdClr    1
#define   lcdSerKi  24
#define   lcdSerKa  28
#define   lcdHome   0
#define   lcdCurKi  16
#define   lcdCurKa  20
#define   lcdCurU   14
#define   lcdCurBL  13
#define   lcdBase1  128
#define   lcdBase2  192

/*Definisi global variabel*/
String  addrDev;
String  inMsg; 
String  dSTX="@";
String  dETX="*"; 
String  dServer="0";
String  dSrc,dDest,dType,dCmd,dData;

float     suhu1,suhu2;
boolean   stsR1,stsR2;

/*=====================================
Definisi object baru untuk Serial LCD
RX pada pin D3
TX pada pin D2
=======================================*/
SoftwareSerial  lcdSerial(4,2);

void setup() {
  /*============================================
  Code ini hanya dijalankan sekali setelah RESET
  ==============================================*/
  /*Tunggu sebentar setelah RESET*/
  delay(500);
  
  /*Konfigurasi pin PANIC-Button*/
  pinMode(btnPanic,INPUT);
  digitalWrite(btnPanic,HIGH);
  attachInterrupt(digitalPinToInterrupt(btnPanic),myEvent,FALLING);

  /*Konfigurasi pin Device Address*/
  pinMode(addrJP1,INPUT);
  pinMode(addrJP0,INPUT);
  digitalWrite(addrJP1,HIGH);
  digitalWrite(addrJP0,HIGH);

  /*Konfigurasi komunikasi serial 9600-N-8-1*/
  Serial.begin(9600);

  /*Konfigurasi komunikasi Serial LCD 9600-N-8-1*/
  lcdSerial.begin(9600);

  /*Konfigurasi Analog Input 10-bit INTERNAL REFERENCE 1V1*/
  analogReference(INTERNAL);

  /*Konfigurasi port I/O disini:*/
  pinMode(inRL1,INPUT);
  pinMode(inRL2,INPUT);
  digitalWrite(inRL1,HIGH);
  digitalWrite(inRL2,HIGH);
  pinMode(otRL1,OUTPUT);
  pinMode(otRL2,OUTPUT);
  pinMode(otBuz,OUTPUT);
  digitalWrite(otRL1,LOW);
  digitalWrite(otRL2,LOW);
  digitalWrite(otBuz,LOW);

  /*Penentuan Device Address berdasarkan JP1 dan JP0*/
  if(digitalRead(addrJP0)==LOW && digitalRead(addrJP1)==LOW)        addrDev="0";
  else if(digitalRead(addrJP0)==HIGH && digitalRead(addrJP1)==LOW)  addrDev="1";
  else if(digitalRead(addrJP0)==LOW && digitalRead(addrJP1)==HIGH)  addrDev="2";
  else if(digitalRead(addrJP0)==HIGH && digitalRead(addrJP1)==HIGH) addrDev="3";

  /*Opening Message*/
  //Inisialisasi LCD
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdClr);
  delay(10);
  lcdSerial.print("Multi Device PRJ");
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase2+2);
  lcdSerial.print("PSAM 2D3 EA");
  delay(500);
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdClr);
  delay(10);
  lcdSerial.print("Dev Number:");
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase2+8);
  lcdSerial.print(addrDev);
  Serial.print(dSTX);
  Serial.print(",");
  Serial.print("Dev");
  Serial.print(",");
  Serial.print(addrDev);
  Serial.print(",");
  Serial.println(dETX);  
  delay(500);
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdClr);
  delay(10);
}

void loop() {
  /*Cek apakah ada data serial yang masuk*/
  if(Serial.available()>0){
   inMsg=Serial.readString();
    /*cek data is valid?*/
    if(inMsg.startsWith(dSTX)==true && inMsg.endsWith(dETX)==true){
      ParsingData();
    }
    else{
      lcdSerial.write(lcdCmd);
      lcdSerial.write(lcdClr);
      delay(10);
      lcdSerial.print("Invalid Msg");
    }
  }
  /*Update data sensor Suhu*/
  suhu1=getTemp(aSuhu1);
  suhu2=getTemp(aSuhu2);

  /*Update aktifasi Relay*/
  stsR1=!digitalRead(inRL1);
  stsR2=!digitalRead(inRL2);
   
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase1);
  lcdSerial.print("                ");
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase1);
  lcdSerial.print(suhu1);
  lcdSerial.print("  ");
  lcdSerial.print(suhu2);
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase1+13);
  lcdSerial.print(stsR1);
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase1+15);
  lcdSerial.print(stsR2);
  delay(500); 
}

/*Fungsi Panic-Button, generate pulse pada Buzzer*/
void myEvent(void){
  digitalWrite(otBuz,HIGH);
  
  /*Update to LCD Serial*/
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdClr);
  delay(10);
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase1);
  lcdSerial.print("  EMERGENCY!!!  ");
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase2);
  lcdSerial.print("  PANIC PRESSED ");

  /*Kirim ke Serial*/
  stsR1=!digitalRead(inRL1);
  stsR2=!digitalRead(inRL2);
  suhu1=getTemp(aSuhu1);
  suhu2=getTemp(aSuhu2);
  
  Serial.print(dSTX);
  Serial.print(",");
  Serial.print(addrDev);
  Serial.print(",");
  Serial.print(dServer);
  Serial.print(",");
  Serial.print("A");
  Serial.print(",");
  Serial.print("PB");
  Serial.print(",");
  Serial.print(suhu1);
  Serial.print(",");
  Serial.print(suhu2);
  Serial.print(",");
  Serial.print(stsR1);
  Serial.print(",");
  Serial.print(stsR2);
  Serial.print(",");
  Serial.println(dETX);
  delay(2000);
  digitalWrite(otBuz,LOW);
}

/*Baca data temperature*/
float getTemp(unsigned int lm35){
  unsigned int adcIn=analogRead(lm35);
  float dSuhu=((float)adcIn/1024)*1.1*100;

  return dSuhu;
}

/*Aktifkan RELAY dan baca status aktifasi RELAY*/
boolean setRelay(unsigned int xRelay, unsigned int yRelay){
  digitalWrite(xRelay,HIGH);

  return digitalRead(yRelay);
}

/*Reset Aktifasi RELAY dan baca status reset aktifasi RELAY*/
boolean rstRelay(unsigned int xRelay, unsigned int yRelay){
  digitalWrite(xRelay,LOW);

  return digitalRead(yRelay);
}

void ParsingData(void){
  /*Lakukan Parsing menggunakan substring*/
  dSrc=inMsg.substring(2,3);
  dDest=inMsg.substring(4,5);
  dType=inMsg.substring(6,7);
  dCmd=inMsg.substring(8,10);
  dData=inMsg.substring(11,12);

  /*Tampilkan ke Dislay*/
  lcdSerial.write(lcdCmd);
  lcdSerial.write(lcdBase2);
  lcdSerial.print(inMsg);
  
  if(dType=="C" && dDest==addrDev){
    if(dCmd=="SR")//Command --> Set Relay
    {
      if(dData=="1"){
        stsR1=setRelay(otRL1,inRL1);
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(!stsR1);
        Serial.print(",");
        Serial.println(dETX);        
      }
      else if(dData=="2"){
        stsR2=setRelay(otRL2,inRL2);
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(!stsR2);
        Serial.print(",");
        Serial.println(dETX);
      }
      else{        
      }
    }
    else if(dCmd=="CR")//Command --> Clear Relay
    {
      if(dData=="1"){
        stsR1=rstRelay(otRL1,inRL1);
        /*Kirim ke Serial*/
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(!stsR1);
        Serial.print(",");
        Serial.println(dETX);
      }
      else if(dData=="2"){
        stsR2=rstRelay(otRL2,inRL2);
        /*Kirim ke Serial*/
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(!stsR2);
        Serial.print(",");
        Serial.println(dETX);
      }
      else{        
      }
    }
    else if(dCmd=="RR")//Command --> Read Relay
    {
      if(dData=="1"){
        stsR1=!digitalRead(inRL1);
        /*Kirim ke serial*/
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(stsR1);
        Serial.print(",");
        Serial.println(dETX);
      }
      else if(dData=="2"){
        stsR2=!digitalRead(inRL2);
        /*Kirim ke Serial*/
        Serial.print(dSTX);
        Serial.print(",");
        Serial.print(addrDev);
        Serial.print(",");
        Serial.print(dServer);
        Serial.print(",");
        Serial.print("R");
        Serial.print(",");
        Serial.print("RR");
        Serial.print(",");
        Serial.print(dData);
        Serial.print(",");
        Serial.print(stsR2);
        Serial.print(",");
        Serial.println(dETX);
      }
      else{        
      }
    }
    else if(dCmd=="GT" && dData=="S")//Command --> Get Temperature
    {
      suhu1=getTemp(aSuhu1);
      suhu2=getTemp(aSuhu2);
      /*Kirim ke Serial*/
      Serial.print(dSTX);
      Serial.print(",");
      Serial.print(addrDev);
      Serial.print(",");
      Serial.print(dServer);
      Serial.print(",");
      Serial.print("R");
      Serial.print(",");
      Serial.print("GT");
      Serial.print(",");
      Serial.print(suhu1);
      Serial.print(",");
      Serial.print(suhu2);
      Serial.print(",");
      Serial.println(dETX);
    }
    else if(dCmd=="GA" && dData=="A")//Command --> Get All Data
    {
      stsR1=!digitalRead(inRL1);
      stsR2=!digitalRead(inRL2);
      suhu1=getTemp(aSuhu1);
      suhu2=getTemp(aSuhu2);
      /*Kirim ke Serial*/
      Serial.print(dSTX);
      Serial.print(",");
      Serial.print(addrDev);
      Serial.print(",");
      Serial.print(dServer);
      Serial.print(",");
      Serial.print("R");
      Serial.print(",");
      Serial.print("GA");
      Serial.print(",");
      Serial.print(suhu1);
      Serial.print(",");
      Serial.print(suhu2);
      Serial.print(",");
      Serial.print(stsR1);
      Serial.print(",");
      Serial.print(stsR2);
      Serial.print(",");
      Serial.println(dETX);
    }
    else
    {
    }
  }
}
