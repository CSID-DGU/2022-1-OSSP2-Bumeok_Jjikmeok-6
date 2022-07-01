# 2022-1-OSSP2-Bumeok_Jjikmeok-6
2022년 1학기 공개SW프로젝트 02분반 6조 부먹찍먹

팀장 : 2018112008 김균호

팀원 : 2018112558 김철희

팀원 : 2018111999 심재혁

----------------------------
### <동국대학교를 게임 내에서 체험해보세요!>
<br>

<p align="center">
  <img src="https://user-images.githubusercontent.com/91325459/172173896-5a615380-3482-4964-82e7-3aa7a36e4418.gif" width="310" height="300"/>
  <img src="https://user-images.githubusercontent.com/91325459/172451264-f951ab74-cb98-4461-9015-5d4e6a9887a6.gif" width="310" height="300"/>
</p>

<p align="center">
  <img src="https://user-images.githubusercontent.com/91325459/172175836-8c6cb236-2ee8-48f9-9d73-9d494819176a.gif" width="310" height="300"/>
  <img src="https://user-images.githubusercontent.com/91325459/172175879-73217b95-1c00-447d-94ad-3e86843997ba.gif" width="310" height="300"/>
</p>

<p align="center">
  <img src="https://user-images.githubusercontent.com/91325459/176866702-db8e733a-3238-41b1-a075-cf1868e93e76.gif" width="310" height="300"/>
  <img src="https://user-images.githubusercontent.com/91325459/172451181-718eaa78-85e1-4176-b371-43fd7f907f36.gif" width="310" height="300"/>
</p>

<p align="center">
  <img src="https://user-images.githubusercontent.com/91325459/172451036-2f0c8b59-70c2-4940-95e5-47bc9850b3a9.gif" width="310" height="300"/>
</p>


<div align="center">[게임 내의 모습]</div>
<br>
게임의 재미가 배가 되도록 동국대학교를 테마로 제작한 게임입니다. 재미있게 즐겨주세요!

----------------------------
### 게임 다운로드
- Easy ver. : https://drive.google.com/file/d/1ekOw5SGV6LVApIzfBKQ-kaWngmcPgNfa/view?usp=sharing
- Hard ver. : https://drive.google.com/file/d/199ymSPnCaJ_sh_MA8x5WR7pTYZebG7Cn/view?usp=sharing
- 두 버전 모두 zip 파일이며, 압축을 푼 후 My project.exe 파일을 실행하여 게임 시작 (2023.4.30 이후로 서버 호스팅 종료 예정)

----------------------------
### 게임 플레이 시 주의 사항
- 이 게임은 인터넷 연결이 필수입니다. 플레이 전 인터넷에 연결해주세요.
- 이 게임은 광과민성 발작을 일으키는 요소가 많으니 주의하시기 바랍니다.

----------------------------
### 게임 구조
<br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172998673-30b69ffb-a955-4160-aff3-38860adb8257.png"/>
</p>

<div align="center">[게임 플로우차트]</div>


- 메인 로비
  - 회원가입 및 로그인 후 게임 시작
  - 로그인 없이 게임 시작 불가능
  - 로그인 유저에 한하여 랭킹 확인 제공
- 스테이지
  - 메인 스테이지 1 - 신공학관
  - 메인 스테이지 2 - 법학/만해관
  - 메인 스테이지 3 - 명진관
  - 최종 스테이지 (1),(2) - 팔정도
- 랭킹 반영
  - 모든 스테이지를 순차적으로 클리어한 이후 랭킹 자동 반영
  - 기존의 랭킹이 전부 최신화
  - 게임 종료 (자동 로그아웃) / 메인 로비 이동 중 선택
- 스토리, 오버뷰
  - 모든 스테이지 이동 시 스토리, 오버뷰 씬 존재
- 설정
  - 모든 스테이지마다 설정 창 존재
  - ESC 키를 눌러 실행
    - 배경음 ON/OFF, 배경음 소리 조절
    - 랭킹 확인
    - 게임 종료 (자동 로그아웃)
    - 메인 로비로 이동



----------------------------
### 개발 환경
<p>
  <img src = "https://img.shields.io/badge/logo-unity v.2020.3.33-green?logo=unity">
  <img src = "https://img.shields.io/badge/logo-express.js-blue?logo=express">
  <img src = "https://img.shields.io/badge/logo-mysql-violet?logo=mysql">
</p>

### 라이선스
<p>
  <img src = "https://img.shields.io/badge/license-GPL%203.0-orange">
</p>

### 사용한 오픈 소스
<p>
https://github.com/himajin-no-tameiki/i-wanna-be-himawari - 최종 스테이지 (2)에서 사용
</p>

----------------------------
### 개발을 위한 유니티 세팅
1. Unity Hub 설치 후, 2020.3.33 LTS 버전을 이어서 다운로드 (버전이 다르면 실행이 안될 가능성이 높음)
2. cmd 창을 열고 해당 프로젝트 파일을 받고 싶은 폴더로 이동 후, 'git clone https://github.com/CSID-DGU/2022-1-OSSP2-Bumeok_Jjikmeok-6.git <폴더 이름 아무거나>' 입력
3. Unity Hub에서 clone한 폴더 내부에 있는 게임 프로젝트 폴더인 'Don't Get Underrated' 실행
4. DOTween (HOTween v2) 무료 버전 다운로드 (https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) (유니티 에셋 스토어에서 제공)
5. **현재 실행된 유니티에서 게임을 실행하기 위해서는 로컬 서버와 데이터베이스 연결이 필수이므로, 아래에 있는 서버 구동 방법에 있는 절차에 따라 서버를 실행**
----------------------------
### 개발을 위한 서버 구동 방법 - 일반적인 방법, 윈도우 기준
1. node.js 설치 (https://nodejs.org/en/, 공식 홈페이지)
2. mysql 설치 (https://dev.mysql.com/downloads/mysql/, 공식 홈페이지)
3. 설치가 완료되면 mysql을 설치한 드라이브 -> Program Files -> MYSQL -> MYSQL Server 8.0 -> bin 경로로 들어가 해당 주소를 복사
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172746280-3e6d8cbb-c1c0-4d78-a8df-c900b293e9aa.png" />
</p>
<div align="center">[3번 과정의 사진]</div>
<br>

4. 시스템 속성 -> 환경 변수 -> 시스템 변수 중 Path 변수 항목 더블 클릭
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172746501-566bad45-f056-47e7-af18-5d33a4bacf4f.png" />
</P>
<div align="center">[4번 과정의 사진]</div>
<br>

5. 환경 변수 편집 -> 새로 만들기 -> 3번 과정에서 복사한 주소 그대로 복사 & 붙여넣기
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172746544-55423eaf-65a9-48d3-8421-5ce22fb919f3.png" />
</p>
<div align="center">[5번 과정의 사진]</div>
<br>

6. **재부팅 후** cmd 창을 열어 본인의 mysql에 접속 (mysql -uroot -p -> mysql을 설치하면서 설정한 비밀번호 입력)
7. (6번에서 연이어 진행) mysql에 접속된 cmd 창에서 "source (clone 폴더 경로)\Unity_Server_general\DB_info.sql" 명령어를 입력하여 데이터베이스 초기 세팅 (ex. source C:\Users\hiwg08\Desktop\2022-1-OSSP2-Bumeok_Jjikmeok-6\Unity_Server_general\DB_info.sql) **(DB_info.sql 파일은 Unity Server (general) 폴더 안에 존재. 4번 과정을 진행하는 시점에서는 DB_info.sql이 위치한 드라이브 == mysql이 설치된 드라이브여야 함. clone한 폴더의 위치는 상관 없음.)**
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172746841-620baafe-0177-4b02-8a22-a50d9f11b5a3.png"/>
</p>
<div align="center">[6, 7번 과정의 사진]</div>
<br>

8. Unity_Server_general/DB_info 디렉토리에서 "secret_OSSW_DB.js" 파일을 새로 생성한 후, mysql을 설치하면서 설정했던 정보를 입력. secret_OSSW_DB.js의 양식은 다음과 같음 <br/> <br/>
module.exports = { <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; host: "mysql에서 설정한 host 주소", (기본 host는 localhost(==127.0.0.1)) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; user: "mysql에서 설정한 user 이름", (기본으로 설정되는 이름은 root) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; password: "mysql에서 설정한 비밀번호", (숫자여도 반드시 큰따옴표로 묶어야함) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; database: "OSSW", (DB이름은 OSSW 고정이므로 수정 X) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; port: mysql의 port 넘버 (기본으로 설정되는 port 넘버는 3306) <br/>
}
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/172746919-04e1f6e4-7485-45ab-a2a9-f8378e18aa91.png"/>
</p>
<div align="center">[8번 과정의 예시 사진 (VSCode 이용)]</div>
<br>

9. cmd 창에서 Unity_Server_general 폴더로 이동 후, npm install 명령어 입력(약간의 시간 소요)
10. 9번에서의 cmd 창에서 npm start 명령어 입력 (cmd 창 대신 VSCode에서 편리하게 명령어를 입력할 수 있음)
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/176899527-852c0531-8396-4722-ac34-5dad2c4d2ce1.png"/>
</p>
<div align="center">[10번 과정의 사진 (VSCode 이용)]</div>
<br>

----------------------------
### 개발을 위한 서버 구동 방법 - 도커 이용 (모든 OS 이용 가능)
1. 도커 설치 (각 OS마다 도커 설치 방법이 다르며, 설치해야 하는 부가 요소들이 상이하므로 주의)
2. Unity_Server_docker 폴더에 위치한 후 (cd Unity_Server_docker), docker compose up 명령어 입력
<br> <br>
<p align="center">
<img src="https://user-images.githubusercontent.com/91325459/176903211-148c003b-9bd2-404b-a3e7-6a1b02c07003.png"/>
</p>
<div align="center">[2번 과정의 사진 (VSCode 이용)]</div>
<br>

----------------------------
### 문의 사항
hiwg08@gmail.com