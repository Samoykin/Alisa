# Alisa
Программа подсчета расходов газа, пара, воды, тепла на НПЗ

Программа рассчитывает параметры по сложным формулам, пишет в БД SQLite. Каждые 2 часа формируется отчет по расходам. Раз в сутки отчеты отправляются по электронной почте на назначенные адреса.
Реализовано резервирование: Master пишет в удаленную БД MSSQL (из нее другая система берет данные). Если по каким-то причинам он этого не сделает, то Slave, запущенный на другом сервере, напишет отчет и отправит его на электронную почту.

![Image alt](https://github.com/Samoykin/Alisa/raw/master/example.png)
