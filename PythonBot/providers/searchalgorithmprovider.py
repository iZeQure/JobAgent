import re
from constants import constant
from datetime import datetime
from bs4 import BeautifulSoup
from services.dataservice import DataService


class SearchAlgorithmProvider:
    data_service: DataService

    title_list = ["datatekniker", "it-support", "infrastruktur", "programmering", "programmør", "support", "it support"]
    companyEmail_list = ["a[href*='mailto']", "a[href^='mailto']"]
    companyPhoneNumber_list = ["td"]
    description_list = ["description", "area body", "article__body", "article_body", "main span"]
    # location_list = ["tr"]
    location_name_filter = ["adresse"]
    # date_list = ["tr"]
    registration_date_name_filters = ["opslagsdato"]
    deadline_date_name_filters = ["ansøgningsfrist", "ansættelsesdato"]
    category_list = ["data-", "kommunikationsuddannelsen"]
    specialization_list = ["programmering"]

    soup = str

    def __init__(self, data_service: DataService):
        self.data_service = data_service

    def set_page_source(self, page_html: str):
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
                return description.get_text(separator=' ')

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_location(self) -> str:
        found_location_element = None
        elements = []

        for location_filter in self.location_name_filter:
            elements.append(self.soup.find(text=re.compile(location_filter, flags=re.IGNORECASE)))

        for arg in elements:
            if arg is not None:
                found_location_element = arg

        if found_location_element is not None:
            actual_location = found_location_element.parent.nextSibling
            if actual_location is not None:
                return actual_location.get_text(separator=' ')

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    def find_registration_date(self) -> str:
        date_time_str = self.get_actual_result_from_element(
            self.get_date_elements(
                self.registration_date_name_filters
            )
        )

        date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        return date_time_obj

    def find_deadline_date(self) -> str:
        date_time_str = self.get_actual_result_from_element(
            self.get_date_elements(
                self.deadline_date_name_filters
            )
        )

        date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        return date_time_obj

    def find_category(self) -> int:
        categories = self.data_service.categories
        result = str

        for category_filter in self.category_list:
            result = self.soup.find(text=re.compile(category_filter, flags=re.IGNORECASE))

        for category in categories:
            if category[1] in result:
                return category[0]

        return 0

    def find_specialization(self) -> int:
        specializations = self.data_service.specializations
        result = str

        for specialization_filter in self.specialization_list:
            result = self.soup.find(text=re.compile(specialization_filter, flags=re.IGNORECASE)).lower()

        for specialization in specializations:
            lowered_spec = str(specialization[1])
            if lowered_spec.lower() in result:
                return specialization[0]

        return 0

    def get_date_elements(self, filter_list: []) -> []:
        elements = []
        for date_filter in filter_list:
            elements.append(self.soup.find(text=re.compile(date_filter, flags=re.IGNORECASE)))

        return elements

    def get_actual_result_from_element(self, elements: []) -> str:
        found_date_element = self.get_result_from_elements(elements)

        if found_date_element is not None:
            date = self.get_date_element_from_parent(found_date_element)
            if date is not None:
                return date

        return constant.SEARCH_ALGORITHM_NOT_FOUND_TEXT

    @staticmethod
    def get_result_from_elements(elements: []) -> []:
        for result in elements:
            if result is not None:
                return result

        return None

    @staticmethod
    def get_date_element_from_parent(date_element: str) -> str:
        sibling_element = date_element.parent.nextSibling

        if sibling_element is not None:
            return sibling_element.text

        return None
