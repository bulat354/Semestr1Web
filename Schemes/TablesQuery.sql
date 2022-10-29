CREATE TABLE Accounts (
	u_id int not null primary key identity(1, 1),
	u_password varchar(50) not null,
	u_name varchar(30) not null,
	u_surname varchar(50) not null,
	u_nickname varchar(50) not null unique,
	u_email varchar(50) not null unique,
	u_tel varchar(20) unique,
	u_gender varchar(20) not null default '',
	u_level varchar(20) not null default '',
	u_birthdate date not null,
	constraint genders check(u_gender in ('', 'apache', 'male', 'female')),
	constraint plevels check(u_level in ('one-c', 'low', 'medium', 'high', 'hacker'))
)

CREATE TABLE Articles (
	a_id int not null primary key identity(1, 1),
	u_id int not null default 0,
	a_title varchar(100) not null,
	a_type varchar(20) not null default 'game',
	a_description text,
	a_firstdate datetime not null default (getdate()),
	a_lastdate datetime not null,
	constraint article_types check(a_type in ('game', 'article')),
	constraint creator foreign key (u_id) references Accounts(u_id) on delete set default,
	constraint lastdatechecker check(a_lastdate >= a_firstdate)
)