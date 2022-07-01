create table Ranking(
    Rank_keycode int(11) not null auto_increment primary key,
    Auth_id int(11) not null,
    main_score_1 int(11) not null,
    main_score_2 int(11) not null,
    main_score_3 int(11) not null,
    final_score_1 int(11) not null,
    final_score_2 int(11) not null,
    index(Auth_id), unique(Auth_id),
    FOREIGN KEY(Auth_id) REFERENCES Auth(keycode) ON DELETE CASCADE);