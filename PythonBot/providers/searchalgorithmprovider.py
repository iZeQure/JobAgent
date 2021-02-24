from constants import constant
import re
from bs4 import BeautifulSoup


class SearchAlgorithmProvider:
    title_list = ["datatekniker", "it-support", "infrastruktur", "programmering", "programmør", "support", "it support"]
    companyEmail_list = ["a[href*='mailto']", "a[href^='mailto']"]
    companyPhoneNumber_list = ["td"]
    description_list = ["description", "area body", "article__body", "article_body", "main span"]
    location_list = ["tr"]
    location_name_filter = ["adresse"]
    regDate_list = ["date"]
    deadlineDate_list = ["h6"]
    category_list = []
    specialization_list = []

    soup = str

    def __init__(self, page_html: str):
        self.soup = BeautifulSoup(page_html, 'html.parser')

    def find_title(self) -> str:
        for arg in self.title_list:
            title = self.soup.find(text=re.compile(arg, flags=re.IGNORECASE))

            if title is not None:
                return title

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_email(self) -> str:
        for arg in self.companyEmail_list:
            mail = self.soup.select_one(arg)

            if mail is not None:
                return mail.get_text()

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_phone_number(self) -> str:
        regex = "([0-9]{8,8})"

        for arg in self.companyPhoneNumber_list:
            for result in self.soup.find_all(arg):
                text_from_result = result.get_text()

                if text_from_result is not None:
                    if re.match(pattern=regex, string=str(text_from_result)):
                        return text_from_result

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_description(self) -> str:
        for arg in self.description_list:
            description = self.soup.select_one(arg)

            if description is not None:
                return description.text

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_location(self) -> str:
        for t in self.soup.find_all(text=re.compile(self.location_name_filter[0], flags=re.IGNORECASE)):
            sibling = t.parent.nextSibling

            if sibling is not None:
                return sibling.text
        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT
