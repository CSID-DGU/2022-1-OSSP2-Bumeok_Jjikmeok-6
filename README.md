# 2022-1-OSSP2-Bumeok_Jjikmeok-6
2022년 1학기 공개SW프로젝트 02분반 6조 부먹찍먹

## 서버 구동 방법
1. cmd 창을 열고 clone한 폴더로 이동하세요. (cmd 창에 "cd <해당 파일 경로>" 입력) (visual studio code 사용 유저는 해당 폴더를 경로로 vscode를 연 후 ctrl + `를 입력하여 나오는 터미널 창에서 명령어를 바로 입력할 수 있습니다.)
2. mysql을 설치하고, cmd 창을 하나 더 열어 본인의 mysql로 접속합니다. (1. mysql -uroot -p / 2.mysql을 설치하면서 설정한 비밀번호 입력)
3. 추가로 연 cmd 창에서 "source (sql이 있는 파일 경로)/DB_info.sql 명령어를 입력하여 데이터베이스를 세팅합니다. (ex. source C:\Users\hiwg08\Desktop\DB_info.sql) (**__sql 파일이 위치한 드라이브는 반드시 mysql이 설치된 드라이브와 일치해야 합니다. 클론한 폴더의 위치는 상관 없습니다.__**)
4. unity_server_main/DB_open/open_OSSW_DB.js에서 password란을 본인이 설정한 mysql 비밀번호로 바꿔주세요. (**__password가 문자열일 때 : " " (큰따옴표), 숫자일 때 : 숫자 입력__**)
5. 기존의 cmd 창에서 npm install 명령어를 입력합니다. (시간이 조금 오래 걸립니다.)
6. 기존의 cmd 창에서 npm start 명령어를 입력합니다. (현재 port 넘버는 3000번이며, unity_server_main/server.js에서 port 넘버를 변경할 수 있습니다.)

## 업데이트 목록
### 게임

### 서버
- 1차 : 2022.04/16 <br/>
- 2차 : 2022.04/17 <br/>
- 3차 : 2022.04/26 <br/>
- 4차 : 2022.04/27 (랭킹 확인 추가)
