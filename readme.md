# My Little Lawyer 
**하나의학사**의 **성인 ADHD의 대처기술 안내서**의 *변호사에게 자문하기* 에서 영감을 얻었고, *사고 기록 양식* 을 작성하여 ADHD의 자동사고를 극복할 수 있도록 도와주는 프로그램이다.

From the greatest book **THE ADULT ADHD TOOL KIT, J. Russell Ramsay & Anthony L. Rostain**, It is inspired by the chapter named *consulting a lawyer* and helps you to write *Accident record form*, then overcome your *automatic think*.

## 클라우드 활용 Utilize Cloud Platform
내가 의도한 대로 최대한으로, 유용하게 사용하는 방법은 클라우드를 사용하는 것으로, 가령 구글 드라이브 데스크탑 등으로 Data 폴더를 동기화 시키는 것 이다. 그리고 여러 컴퓨터에서 Data를 프로그램에 참조하게 하여 사용하라.  

As I designed, at the highly, The way to use this is using cloud platform, such as Google Drive Desktop and synchronize Data folder. and then just make programs at other computers refer Data folder.

[Google Drive Desktop](https://www.google.com/drive/download/)  
[Dropbox](https://www.dropbox.com/downloading)

## 사용법 - 개발자용 Usage for Developers
My Little Lawyer 는 Console Application이다.그리고 Newtonsoft Json 을 사용한다.  
프로그램의 실행 파일과 동일한 위치에 settings.txt 파일을 생성하라. 

My Little Lawyer is Console Application. and it use Newtonsoft Json. Anyway create settings.txt file at the same directory with program.exe is  

### settings.txt 문법 Syntax
```Key=(vValue, Value2, ...)``` 가 한 문장을 차지한다. 고로, ```Folder``` Key로 클라우드 폴더(Data로 명명)의 위치를 적어놓아라. 프로그램에서 명령을 하달할때마다 기록을 보고싶지 않으면 ```Showlog``` Key를 ```False```로 설정하라.  
윗 예시에서 볼 수 있듯이 여러가지 값을 써놓을 수 있고, 실제 사용할 값에는 앞에 v를 적어놓아라.  
값이 소문자 ```v```로 시작할일은 없도록 했다. v를 쓰지 않아도 되는 경우는 Value가 하나 밖에 없을 떄 이다.

```Key=(vValue, Value2, ...)``` configures a line. Accordingly, write a key named ```Folder``` and set value a text that contains path of cloud folder named Data. If you don't want to see logs every single command typed, then Set value of ```Showlog``` to ```False```.
As above imply, you can write many values at one key. But you should put a lower case character ```v``` in front of value on wanting to use.