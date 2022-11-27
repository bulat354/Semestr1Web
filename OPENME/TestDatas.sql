SET IDENTITY_INSERT [dbo].[Images] ON
INSERT INTO [dbo].[Images] ([Id], [Name], [ColorHex], [Alt]) VALUES (1, N'war-of-god.jpg', N'#171816', N'War of Gods')
INSERT INTO [dbo].[Images] ([Id], [Name], [ColorHex], [Alt]) VALUES (3, N'cyberpunk.jpg', N'#050d37', N'KyberPink 3077')
INSERT INTO [dbo].[Images] ([Id], [Name], [ColorHex], [Alt]) VALUES (4, N'top10.jpg', N'#202a21', N'Top 10')
INSERT INTO [dbo].[Images] ([Id], [Name], [ColorHex], [Alt]) VALUES (5, N'what-to-play.jpg', N'#1f292a', N'What To Play')
INSERT INTO [dbo].[Images] ([Id], [Name], [ColorHex], [Alt]) VALUES (6, N'programmers.jpg', N'#271b2b', N'Programmers')
SET IDENTITY_INSERT [dbo].[Images] OFF

SET IDENTITY_INSERT [dbo].[Articles] ON
INSERT INTO [dbo].[Articles] ([Id], [Title], [Content], [Date], [ImageId]) VALUES (2, N'Топ 10 игровых студий по мнению Gavno Games', 
N'Наша команда со всей ответственностью подошла к этому вопросу и подготовила для вас топ самых любимых и вообще лучших игровых студий в мире.
На первом месте по нашему скромному мнению находятся Gavno Games. На втором Microsoft. 
На третьем расположились Sony. 
Про остальные места мне лень писать, так что обсудите этот вопрос в комментариях.', N'2022-11-26 12:30:00', 4)
INSERT INTO [dbo].[Articles] ([Id], [Title], [Content], [Date], [ImageId]) VALUES (3, N'Во что поиграть в 2022 году?', 
N'Многие задаются вопросом: Во что же поиграть в этом, не самом хорошем году, чтобы хоть как-то отвлечься от ситуации в мире? 
Наша команда выбрала для вас во что поиграть лучше всего: конечно же в наши игры! War of Gods и KyberPink 3077 здорово укарсят ваш вечер. Эти игры вы можете посмотреть на отдельной странице. 
Не забудьте поделиться своими впечатлениями в комментариях. ', N'2022-11-26 12:35:00', 5)
INSERT INTO [dbo].[Articles] ([Id], [Title], [Content], [Date], [ImageId]) VALUES (4, N'Мы появились!', 
N'Мы появились! Добро пожаловать на наш сайт. Всем читателям привет!', N'2022-11-25 13:00:00', 6)
SET IDENTITY_INSERT [dbo].[Articles] OFF

SET IDENTITY_INSERT [dbo].[Games] ON
INSERT INTO [dbo].[Games] ([Id], [Title], [Description], [Price], [ImageId], [Date]) VALUES (1, N'War of Gods', 
N'Какие испытания приготовили для несмеренного бога битв Братосу? Сможет ли он спасти жизнь своему сыну Андрею?', CAST(100.0000 AS Money), 1, N'2022-11-26 10:00:00')
INSERT INTO [dbo].[Games] ([Id], [Title], [Description], [Price], [ImageId], [Date]) VALUES (2, N'KyberPink 3077', 
N'В борьбе с мегакорпорациями Ди словил себе на голову Денни Голденхенда. Выживут ли они в мрачном мире Киберпанка', CAST(98.0000 AS Money), 3, N'2022-11-26 10:24:00')
SET IDENTITY_INSERT [dbo].[Games] OFF