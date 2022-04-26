## 서버 구동 방법
1. cmd 창을 열고, 해당 파일 디렉토리로 이동하세요. (cmd 창에 "cd <해당 파일 경로>" 입력) (visual studio code 사용 유저는 vscode를 연 후, 해당 파일에서 ctrl + ` 입력 시 터미널 창이 뜨며, 해당 파일에서 명령어를 바로 입력할 수 있습니다.)
2. mysql을 설치하고, cmd 창을 통해 본인의 mysql로 접속합니다. (mysql -uroot -p -> 설치하면서 설정한 비밀번호 입력)
3. "source (sql이 있는 파일 경로)/DB_info.sql 명령어 입력 후 파일을 불러옵니다. (ex. source C:\Users\hiwg08\Desktop\DB_info.sql) (**** sql 파일이 위치한 드라이브는 반드시 mysql이 설치된 드라이브와 일치해야 합니다.)
4. DB_open/open_OSSW_DB.js로 들어가 password를 본인의 mysql 비밀번호로 바꿔주세요.
5. npm install 명령어를 입력합니다.
6. npm start 명령어를 입력합니다. (node server.js / port = 3000, port는 임의로 변경 가능합니다.)

## 업데이트 목록
서버 1차 : 2022.04/16 <br/>
서버 2차 : 2022.04/17 <br/>
서버 3차 : 2022.04/26 <br/>
서버 4차 : 2022.04/27 (랭킹 확인 추가)