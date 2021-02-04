import requests
import pyodbc
from bs4 import BeautifulSoup
from datetime import date
from dateutil.relativedelta import relativedelta
import dateparser
import re

title_list = ["title", "article__title", "article_title", "md-headline", "h1", "h2"]
companyEmail_list = []
companyPhoneNumber_list = []
description_list = ["description", "area body", "article__body", "article_body", "p"]
location_list = ["location", "area", "city", "area company flex-gt-sm-30 flex-100"]
regDate_list = ["date"]
deadlineDate_list = ["h6"]
category_list = []
specialization_list = []


def find(check_list, return_type):
    try:
        for item in check_list:
            x = soup.find(id=str(item))
            if str(x) != 'None':
                return x.get_text()
            else:
                y = soup.find(class_=str(item))
                if str(y) != 'None':
                    return y.get_text()
                else:
                    z = soup.find(item)
                    if str(z) != 'None':
                        return z.get_text()
        if return_type == 's':
            return 'Ikke fundet'
        elif return_type == 'i':
            return 0
        elif return_type == 'rd':
            return date.today()
        elif return_type == 'dd':
            return date.today() + relativedelta(months=1) + relativedelta(days=1)

        # file = open('html.txt', 'r')
        # read = file.readlines()
        # file.close()
        # for searchword in list:
        #     lower = searchword.lower()
        #     for word in read:
        #         line = word.split()
        #         for each in line:
        #             line2 = each.lower()
        #             line2 = line2.strip("<>()[]{}/\\=?!-*¨^'.,")
        #             if lower == line2:
        #                 return lower

    except ValueError:
        if return_type == 's':
            return 'Ikke fundet'
        elif return_type == 'i':
            return 0
        elif return_type == 'rd':
            return date.today()
        elif return_type == 'dd':
            return date.today() + relativedelta(months=1) + relativedelta(days=1)


conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                      'Server=JOB-AGENT\\SQLJOBAGENT;'
                      'Database=JobAgentDB;'
                      'UID=sa;'
                      'PWD=PaSSw0rd;'
                      'Trusted_Connection=yes')

cursor = conn.cursor()

sourceLinks = list(cursor.execute('SELECT * FROM SourceLink'))

jobAdvertCheck = list(cursor.execute('SELECT SourceURL FROM JobAdvert'))

for sourceLink in sourceLinks:
    newJobAdvert = True
    for jobAdvertURL in jobAdvertCheck:
        if jobAdvertURL[0] == sourceLink[2]:
            newJobAdvert = False
            break
    if newJobAdvert:
        # print(sourceLink)
        companyID = sourceLink[1]
        url = sourceLink[2]
        page = requests.get(url)
        data = page.text
        soup = BeautifulSoup(data, 'html.parser')

        # with open('html.txt', 'w') as file:
        #     file.write(str(soup.encode('utf-8')))
        # print(file)

        title = find(title_list, 's')
        companyEmail = find(companyEmail_list, 's')
        companyPhoneNumber = find(companyPhoneNumber_list, 's')
        description = find(description_list, 's')
        location = find(location_list, 's')
        if "/" in str(find(regDate_list, 'rd')):
            regDate = str(find(regDate_list, 'rd').replace("/", "-"))
        else:
            regDate = str(find(regDate_list, 'rd'))

        regDate = re.findall("[0-9]|-", regDate)
        regDate = ''.join(regDate)
        regDate = dateparser.parse(regDate)

        # regDate = regDate.strip("q", "w", "e", "t", "y", "u", "i", "o", "p", "å", "a", "s", "d", "f", "g", "h", "j", "k", "l", "æ", "ø", "z", "x", "c", "v", "b", "n", "m")
        # regDate = soup.find(class_='date').get_text()
        # regDate = regDate.split('/')
        # regDate = datetime.date(int(regDate[2]), int(regDate[1]), int(regDate[0]))
        # d = datetime.strptime(regDate, '%d-%m-%Y')
        # regDate = d.strftime('%m-%d-%Y')
        if "/" in str(find(deadlineDate_list, 'dd')):
            deadlineDate = str(find(deadlineDate_list, 'dd').replace("/", "-"))
        else:
            deadlineDate = str(find(deadlineDate_list, 'dd'))

        deadlineDate = re.findall("[0-9]|-", deadlineDate)
        deadlineDate = ''.join(deadlineDate)
        deadlineDate = dateparser.parse(deadlineDate)

        # deadlineDate = deadlineDate.split('-')
        # deadlineDate = datetime.date(int(deadlineDate[2]), int(deadlineDate[1]), int(deadlineDate[0]))
        # d = datetime.strptime(deadlineDate, '%d-%m-%Y') + relativedelta(years=5)
        # deadlineDate = d.strftime('%Y-%m-%d')

        categoryId = find(category_list, "i")
        specializationId = find(specialization_list, "i")

        sql = f"EXEC [CreateJobAdvert] '{str(title)}', '{str(companyEmail)}', '{str(companyPhoneNumber)}'," \
              f"'{str(description)}', '{str(location)}', '{str(regDate)}', '{str(deadlineDate)}', '{str(url)}'," \
              f"{str(companyID)}, {int(categoryId)}, {int(specializationId)}"
        cursor.execute(sql)

cursor.commit()
