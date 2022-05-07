create database OSSW;
use OSSW;
create table Auth(
    keycode int(11) not null auto_increment primary key,
    id text not null,
    pwd text not null);
create table Ranking(
    Rank_keycode int(11) not null auto_increment primary key,
    Auth_id int(11) not null,
    score1 int(11) not null,
    score2 int(11) not null,
    score3 int(11) not null,
    index(Auth_id),
    FOREIGN KEY(Auth_id) REFERENCES Auth(keycode) ON DELETE CASCADE);

