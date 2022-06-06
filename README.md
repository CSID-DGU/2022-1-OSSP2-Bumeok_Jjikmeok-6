# 2022-1-OSSP2-Bumeok_Jjikmeok-6
2022년 1학기 공개SW프로젝트 02분반 6조 부먹찍먹

팀장 : 2018112008 김균호

팀원 : 2018112558 김철희

팀원 : 2018111999 심재혁

----------------------------
### <동국대학교를 게임 내에서 체험해보세요!>

<img src="https://user-images.githubusercontent.com/91325459/172173896-5a615380-3482-4964-82e7-3aa7a36e4418.gif" width="310" height="300" align="left"/>
<img src="https://user-images.githubusercontent.com/91325459/172175836-8c6cb236-2ee8-48f9-9d73-9d494819176a.gif" width="310" height="300" align="left"/>
<img src="https://user-images.githubusercontent.com/91325459/172175879-73217b95-1c00-447d-94ad-3e86843997ba.gif" width="310" height="300" align="left"/>
<img src="https://user-images.githubusercontent.com/91325459/172175910-71a90687-74d7-4539-abed-a04d49a17891.gif" width="310" height="300"/>

<div align="center">[게임 내의 모습]</div>
<br>
게임의 재미가 배가 되도록 동국대학교를 테마로 제작한 게임입니다. 재미있게 즐겨주세요!

----------------------------
### **게임 플레이 시 주의 사항**
- 이 게임은 서버를 실행시킨다는 전제 하에 플레이가 가능하니, 아래에 있는 서버 구동 방법에 있는 절차를 따라 서버를 실행시키세요. 
- 이 게임은 광과민성 발작을 일으키는 요소가 상당히 많으니 주의하시기 바랍니다.

----------------------------
### 개발 환경
<p>
  <img src = "https://img.shields.io/badge/logo-unity v.2020.3.33-green?logo=unity">
  <img src = "https://img.shields.io/badge/logo-node.js-blue?logo=node.js">
  <img src = "https://img.shields.io/badge/logo-mysql-violet?logo=mysql">
</p>

### 라이선스
<p>
  <img src = "https://img.shields.io/badge/license-GPL%203.0-orange">
</p>

### 사용한 오픈 소스
<p>
https://github.com/himajin-no-tameiki/i-wanna-be-himawari
</p>

----------------------------
### 개발을 위한 유니티 세팅
1. Unity Hub 설치 후, 2020.3.33 LTS 버전을 이어서 다운로드 (버전이 다르면 실행이 안될 가능성이 높음)
2. cmd 창을 열고 해당 프로젝트 파일을 받고 싶은 폴더로 이동 후, 'git clone https://github.com/CSID-DGU/2022-1-OSSP2-Bumeok_Jjikmeok-6.git <폴더 이름 아무거나>' 입력
3. Unity Hub에서 clone한 폴더 내부에 있는 게임 프로젝트 폴더인 'Don't Get Underrated' 실행
4. DOTween (HOTween v2) 무료 버전 다운로드 (https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) (유니티 에셋 스토어에서 제공)

----------------------------
### 개발을 위한 서버 구동 방법
1. mysql을 설치하고 cmd 창을 하나 더 열어 본인의 mysql에 접속 (mysql -uroot -p -> mysql을 설치하면서 설정한 비밀번호 입력)
2. (1번에서 연이어 진행) mysql에 접속된 cmd 창에서 "source (clone 폴더 경로)\unity_server_main\DB_info.sql" 명령어를 입력하여 데이터베이스 초기 세팅 (ex. source C:\Users\hiwg08\Desktop\2022-1-OSSP2-Bumeok_Jjikmeok-6\unity_server_main\DB_info.sql) **(DB_info.sql 파일은 unity_server_main 폴더 안에 존재. 4번 과정을 진행하는 시점에서는 DB_info.sql이 위치한 드라이브 == mysql이 설치된 드라이브여야 함. clone한 폴더의 위치는 상관 없음.)**
3. unity_server_main/DB_info 디렉토리에서 "secret_OSSW_DB.js" 파일을 새로 생성한 후, mysql을 설치하면서 설정했던 정보를 입력. secret_OSSW_DB.js의 양식은 다음과 같음 <br/> <br/>
module.exports = { <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; host: 127.0.0.1, <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; user: (mysql의 user 이름), <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; password: (mysql에서 설정한 비밀번호), <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; database: "OSSW", (DB이름은 OSSW 고정) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; port: (mysql의 port 넘버) <br/>
}
<br><br>
4. cmd 창에서 unity_server_main 폴더로 이동 후, npm install 명령어 입력(약간의 시간 소요)
5. 4번에서의 cmd 창에서 npm start 명령어 입력(현재 port 넘버는 3000번이며, unity_server_main/server.js에서 port 넘버 변경 가능 (const port 부분에서 수정). VSCode에서 편리하게 명령어 입력 가능)

----------------------------
### 문의 사항
lateson6@gmail.com