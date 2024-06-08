#Так выглядит в docker-desktop (контейнер senderemail почемуто падает, но после нескольких рестартов работает (возможно rabbitmq не успевает настроиться) )
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/35d8de0a-144c-49e7-9787-104fb0620cf8)

#Так же тут включил opensearch (делал логи через serilog)
#Далее приведен скрин из opensearch-dashboards
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/eb087840-138b-49b9-b0b4-b6892cb54c2c)

#Сделал отправку на сервер файлов (в папку FilesUsers)
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/134b6b8b-fabe-4ddf-b660-60c2d0f6917d)

#так же использовал redis для кеширования файлов и пользователей зарегестрировавшихся или залогинившихся (для файлов key - сгенерированное имя файла value - относительный путь 
#для пользователей key - почта value - json строка пользователя)
#Пример кеша для файла 
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/4dd8b2e8-431e-420c-8ee7-e1602dce5a58)
#Пример кеша для пользователя
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/99a2a000-7921-41f6-a6d5-7581219a6d72)

#Использовал rabbitmq для общения между основным приложением и отдельным приложением senderemail (который принимает emailDto и использую его отправляет на почту)
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/50dff7c2-1703-4b5b-a169-6ac31bb1fe5a)
#Как выглядит на почте 
![image](https://github.com/stalkeronag/course_asp_net/assets/86604604/aaed2bbd-91b0-409f-b666-84dd4308a76f)


