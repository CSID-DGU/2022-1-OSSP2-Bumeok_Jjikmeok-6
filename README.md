# 2022-1-OSSP2-Bumeok_Jjikmeok-6
2022년 1학기 공개SW프로젝트 02분반 6조 부먹찍먹

## **게임 플레이 시 주의 사항**
- 이 게임은 반드시 서버를 실행시킨다는 전제하에 플레이가 가능하니, 아래에 있는 서버 구동 방법에 있는 절차를 따라 서버를 실행시키세요. 
- 이 게임은 광과민성 발작을 일으키는 요소가 다분하니, 주의하시기 바랍니다.

## 서버 구동 방법
1. cmd 창을 열고 clone한 폴더의 unity_server_main 폴더로 이동하세요. (cmd 창에 "cd <해당 파일 경로>" 입력) (ex. cd C:\Users\hiwg08\Desktop\2022-1-OSSP2-Bumeok_Jjikmeok-6\unity_server_main)
2. mysql을 설치하고, cmd 창을 하나 더 열어 본인의 mysql로 접속합니다. (1. mysql -uroot -p / 2.mysql을 설치하면서 설정한 비밀번호 입력)
3. (2번에서 연이어 진행합니다.) 데이터베이스에 접속된 cmd 창에서 "source (clone 폴더 경로)\unity_server_main\DB_info.sql" 명령어를 입력하여 데이터베이스를 세팅합니다. (ex. source C:\Users\hiwg08\Desktop\2022-1-OSSP2-Bumeok_Jjikmeok-6\unity_server_main\DB_info.sql) **(sql 파일(DB_info.sql)이 위치한 드라이브는 반드시 mysql이 설치된 드라이브와 일치해야 합니다. clone한 폴더의 위치는 상관 없습니다.)**
4. unity_server_main/DB_info 디렉토리에서 "secret_OSSW_DB.js" 파일을 생성한 후, mysql을 설치하면서 설정했던 정보를 입력해주세요. secret_OSSW_DB.js의 양식은 다음과 같습니다. <br/> <br/>
module.exports = { <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; host: (mysql의 host 주소), <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; user: (mysql의 user 이름), <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; password: (mysql에서 설정한 비밀번호), <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; database: "OSSW", (DB이름은 OSSW 고정입니다.) <br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; port: (mysql의 port 넘버) <br/>
}
5. 기존의 cmd 창에서 npm install 명령어를 입력합니다. (약간의 시간 소요)
6. 기존의 cmd 창에서 npm start 명령어를 입력합니다. (현재 port 넘버는 3000번이며, unity_server_main/server.js에서 port 넘버를 변경할 수 있습니다 (const port 부분에서 수정).

## 업데이트 목록
### 게임

### 서버
- 1차 : 2022.04/16 <br/>
- 2차 : 2022.04/17 <br/>
- 3차 : 2022.04/26 <br/>
- 4차 : 2022.04/27
- 5차 : 2022.04/28
