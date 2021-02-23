import requests
import dateparser
import re
from datetime import date
from dateutil.relativedelta import relativedelta
from bs4 import BeautifulSoup
from config import DatabaseConfig

title_list = ["title", "article__title", "article_title", "md-headline", "h1", "h2"]
companyEmail_list = []
companyPhoneNumber_list = []
description_list = ["description", "area body", "article__body", "article_body", "p"]
location_list = ["location", "area", "city", "area company flex-gt-sm-30 flex-100"]
regDate_list = ["date"]
deadlineDate_list = ["h6"]
category_list = []
specialization_list = []


class SearchAlgorithm:
    dbConnection = DatabaseConfig.connection
    soup = object
    jobListingUrls = object
    existingJobListingUrls = object

    def __init__(self):
        print('Search Algorithm Initialized.')
        self.load_data_elements_for_search_algorithm()

    def load_data_elements_for_search_algorithm(self):
        """
        Loads the data to use for the search algorithm.
        """

        print("Loading elements..")

        try:
            self.jobListingUrls = list(self.dbConnection.cursor.execute('SELECT * FROM [SourceLink]'))
            self.existingJobListingUrls = list(self.dbConnection.cursor.execute('SELECT [SourceUrl] FROM [JobAdvert]'))
        except ValueError:
            return 'Uncaught Exception'

    def search_for_match(self, check_list, return_type):
        """
        Searches for a match by the given list.
        @param check_list: A list to find matches in.
        @param return_type: Defines the type to be returned.
        @return: Either a string, integer or a date.
        """

        try:
            for item in check_list:
                x = self.soup.find(id=str(item))
                if str(x) != 'None':
                    return x.get_text()
                else:
                    y = self.soup.find(class_=str(item))
                    if str(y) != 'None':
                        return y.get_text()
                    else:
                        z = self.soup.find(item)
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

        except ValueError:
            if return_type == 's':
                return 'Ikke fundet'
            elif return_type == 'i':
                return 0
            elif return_type == 'rd':
                return date.today()
            elif return_type == 'dd':
                return date.today() + relativedelta(months=1) + relativedelta(days=1)

    def save_match_in_db(self):
        for sourceLink in list(self.jobListingUrls):
            new_job_advert = True
            for jobAdvertURL in list(self.existingJobListingUrls):
                if jobAdvertURL[0] == sourceLink[2]:
                    new_job_advert = False
                    break
            if new_job_advert:
                company_id = sourceLink[1]
                url = sourceLink[2]
                page = requests.get(url)
                data = page.text
                self.soup = BeautifulSoup(data, 'html.parser')

                title = self.search_for_match(title_list, 's')
                company_email = self.search_for_match(companyEmail_list, 's')
                company_phone_number = self.search_for_match(companyPhoneNumber_list, 's')
                description = self.search_for_match(description_list, 's')
                location = self.search_for_match(location_list, 's')
                if "/" in str(self.search_for_match(regDate_list, 'rd')):
                    registration_date = str(self.search_for_match(regDate_list, 'rd').replace("/", "-"))
                else:
                    registration_date = str(self.search_for_match(regDate_list, 'rd'))

                registration_date = self.date_parser(registration_date)

                if "/" in str(self.search_for_match(deadlineDate_list, 'dd')):
                    deadline_date = str(self.search_for_match(deadlineDate_list, 'dd').replace("/", "-"))
                else:
                    deadline_date = str(self.search_for_match(deadlineDate_list, 'dd'))

                deadline_date = self.date_parser(deadline_date)

                category_id = self.search_for_match(category_list, "i")
                specialization_id = self.search_for_match(specialization_list, "i")

                sql = f"EXEC [CreateJobAdvert] '{str(title)}', '{str(company_email)}', '{str(company_phone_number)}'," \
                      f"'{str(description)}', '{str(location)}', '{str(registration_date)}', '{str(deadline_date)}', '{str(url)}'," \
                      f"{str(company_id)}, {int(category_id)}, {int(specialization_id)}"
                self.dbConnection.execute(sql)

        self.dbConnection.commit()

    @staticmethod
    def date_parser(registration_date):
        registration_date = re.findall("[0-9]|-", registration_date)
        registration_date = ''.join(registration_date)
        registration_date = dateparser.parse(registration_date)
        return registration_date
