from time import sleep
from re import match
from requests import request
from datetime import date
from dateutil.relativedelta import relativedelta
from selenium import webdriver

from objects.jobadvert import JobAdvert
from providers.searchalgorithmprovider import SearchAlgorithmProvider


class AlgorithmService:
    raw_data = []

    def get_raw_data(self, source_links: []) -> []:
        driver = webdriver.Firefox(executable_path='C:\\VirtualEnvironment\\geckodriver.exe')
        driver.minimize_window()

        for url in source_links:
            formatted_url = self.format_url(url[1])

            # page_response_body = request("get", formatted_url)
            # html_data = page_response_body.text

            driver.get(formatted_url)
            sleep(1)
            self.raw_data.append(str(driver.page_source))

        driver.quit()
        return self.raw_data

    @staticmethod
    def format_url(url: str):
        if not match('(?:http|ftp|https)://', url):
            return 'https://{}'.format(url)
        return url

    @staticmethod
    def find_jobadvert_match(raw_data: str) -> JobAdvert:
        provider = SearchAlgorithmProvider(raw_data)

        jobadvert = JobAdvert(0, '', '', '', '', '', date.today(), date.today() + relativedelta(years=5), '', 0, 0, 0)

        jobadvert.title = provider.find_title()
        jobadvert.email = provider.find_email()
        jobadvert.phone_number = provider.find_phone_number()
        jobadvert.description = provider.find_description()
        jobadvert.location = provider.find_location()

        print(jobadvert.print())

        return jobadvert
