FROM mongo:3.6.4

ENV AUTH yes
ENV STORAGE_ENGINE wiredTiger
ENV JOURNALING yes

ADD run.sh /run.sh
RUN ["chmod", "+x", "/run.sh"]
ADD set_mongodb_password.sh /set_mongodb_password.sh
RUN ["chmod", "+x", "/set_mongodb_password.sh"]

CMD ["/run.sh"]

