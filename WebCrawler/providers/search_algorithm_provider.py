import re
from datetime import datetime

from bs4 import BeautifulSoup

from WebCrawler.constants.constant import Constant
from WebCrawler.services.data_service import DataService


class SearchAlgorithmProvider:
    _data_service: DataService
    _constant: Constant

    _title_filter_keys = []
    _email_filter_keys = []
    _phone_number_filter_keys = []
    _description_filter_keys = []
    _location_name_filter_keys = []
    _registration_date_name_filter_keys = []
    _deadline_date_name_filter_keys = []
    _category_filter_keys = []
    _specialization_filter_keys = []

    _soup = str

    def __init__(self, data_service: DataService, constant: Constant):
        self._data_service = data_service
        self._constant = constant
        self.load_filters()

    def load_filters(self):
        self._title_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Title_Key"))
        self._email_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Email_Key"))
        self._phone_number_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Phone_Number_Key"))
        self._description_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Description_Key"))
        self._location_name_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Location_Key"))
        self._registration_date_name_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Registration_Date_Key"))
        self._deadline_date_name_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Deadline_Date_Key"))
        self._category_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Category_Key"))
        self._specialization_filter_keys = \
            self.get_keys_from_list(self._data_service.get_keys_by_value("Specialization_Key"))

    @staticmethod
    def get_keys_from_list(keys: []) -> []:
        temp_list = []
        for key in keys:
            temp_list.append(key[0])
        return temp_list

    def set_page_source(self, page_html: str):
        self._soup = BeautifulSoup(page_html, 'html.parser')

    def find_title(self) -> str:
        for arg in self._title_filter_keys:
            title = self._soup.find(text=re.compile(arg, flags=re.IGNORECASE))

            if title is not None:
                return title

        return self.__get_notfound_text__()

    def find_email(self) -> str:
        for arg in self._email_filter_keys:
            mail = self._soup.select_one(arg)

            if mail is not None:
                return mail.get_text()

        return self.__get_notfound_text__()

    def find_phone_number(self) -> str:
        regex = "([0-9]{8,8})"

        for arg in self._phone_number_filter_keys:
            for result in self._soup.find_all(arg):
                text_from_result = result.get_text()

                if text_from_result is not None:
                    if re.match(pattern=regex, string=str(text_from_result)):
                        return text_from_result

        return self.__get_notfound_text__()

    def find_description(self) -> str:
        for arg in self._description_filter_keys:
            description = self._soup.select_one(arg)

            if description is not None:
                return description.get_text(' ', True)

        return self.__get_notfound_text__()

    def find_location(self) -> str:
        found_location_element = None
        elements = []

        for location_filter in self._location_name_filter_keys:
            elements.append(self._soup.find(text=re.compile(location_filter, flags=re.IGNORECASE)))

        for arg in elements:
            if arg is not None:
                found_location_element = arg

        if found_location_element is not None:
            actual_location = found_location_element.parent.nextSibling
            if actual_location is not None:
                return actual_location.get_text(separator=' ')

        return self.__get_notfound_text__()

    def find_registration_date(self) -> datetime:
        date_time_str = self.get_actual_result_from_element(
            self.get_date_elements(
                self._registration_date_name_filter_keys
            )
        )

        date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        return date_time_obj

    def find_deadline_date(self) -> datetime:
        date_time_str = self.get_actual_result_from_element(
            self.get_date_elements(
                self._deadline_date_name_filter_keys
            )
        )

        date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        return date_time_obj

    def find_category(self) -> int:
        category_filter_keys = self._category_filter_keys
        categories = self._data_service.get_categories()
        category_id = 0
        result = ""

        for category_filter_key in category_filter_keys:
            result = self._soup.find(text=re.compile(category_filter_key, flags=re.IGNORECASE))
            if result is not None:
                result = result.lower()

        for i, category in enumerate(categories):
            if str(category[1]).lower() in result:
                category_id = category[0]
                break

        if category_id != 0:
            return category_id

        return self.__get_notfound_identifier()

    def find_specialization(self) -> int:
        specialization_filter_keys = self._specialization_filter_keys
        specializations = self._data_service.get_specializations()
        specialization_id = 0

        for specialization_filter in specialization_filter_keys:
            result = self._soup.find(text=re.compile(specialization_filter, flags=re.IGNORECASE))
            if result is not None:
                result = result.lower()
                for specialization in specializations:
                    lowered_spec = str(specialization[1])
                    if lowered_spec.lower() in result:
                        specialization_id = specialization[0]
                        break
            else:
                continue

        if specialization_id != 0:
            return specialization_id

        return self.__get_notfound_identifier()

    def get_date_elements(self, filter_list: []) -> []:
        elements = []
        for date_filter in filter_list:
            elements.append(self._soup.find(text=re.compile(date_filter, flags=re.IGNORECASE)))

        return elements

    def get_actual_result_from_element(self, elements: []) -> str:
        """
        Notes:
            Gets the actual date result of the elements.
        Args:
            elements: An array of elements, containing a date.
        Returns:
                str: Contains the actual date as a string.
        """
        found_date_element = self.get_result_from_elements(elements)

        if found_date_element is not None:
            date = self.get_date_element_from_parent(found_date_element)
            if date is not None:
                return date

        return self.__get_notfound_text__()

    def get_result_from_elements(self, elements: []) -> []:
        """
        Summary:
            Gets a result from the elements.
        Args:
            elements: An array of elements containing data.
        Returns:
            str: Returns a result of the elements if not None, else Not Found.
        """
        for result in elements:
            if result is not None:
                return result

        return self.__get_notfound_text__()

    def get_date_element_from_parent(self, date_element) -> str:
        """
        Summary:
            Extracts the date element from the parent as next sibling.
        Args:
            date_element: The parent, of which has a sibling of a date element.
        Returns:
            str: The extracted element, if not None, else Not Found message.
        """
        sibling_element = date_element.parent.nextSibling

        if sibling_element is not None:
            return sibling_element.text

        return self.__get_notfound_text__()

    def __get_notfound_text__(self) -> str:
        return self._constant.get_notfound_text()

    def __get_notfound_identifier(self) -> int:
        return self._constant.get_notfound_identifier()
