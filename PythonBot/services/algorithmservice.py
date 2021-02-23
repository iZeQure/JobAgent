from re import match
from requests import request
from datetime import date
from objects.jobadvert import JobAdvert
import dateparser
from dateutil.relativedelta import relativedelta
from bs4 import BeautifulSoup


class AlgorithmService:
    raw_data = []

    def get_raw_data(self, source_links: []) -> []:
        for url in source_links:
            formatted_url = self.format_url(url[1])
            page_response_body = request("get", formatted_url)
            html_data = page_response_body.text
            self.raw_data.append(str(html_data))

        return self.raw_data

    @staticmethod
    def format_url(url: str):
        if not match('(?:http|ftp|https)://', url):
            return 'https://{}'.format(url)
        return url

    def find_jobadvert_match(self, raw_data: str) -> JobAdvert:
        j = JobAdvert(int(1), 'Test', 'test@crawler.net', '12345678', 'test', 'nowhere',
                      date.today(), date.today() + relativedelta(years=5), 'google.dk',
                      1, 0, 0)
        return j


class SearchPatternService:
    title_list = ["title", "article__title", "article_title", "md-headline", "h1", "h2"]
    companyEmail_list = []
    companyPhoneNumber_list = []
    description_list = ["description", "area body", "article__body", "article_body", "p"]
    location_list = ["location", "area", "city", "area company flex-gt-sm-30 flex-100"]
    regDate_list = ["date"]
    deadlineDate_list = ["h6"]
    category_list = []
    specialization_list = []
