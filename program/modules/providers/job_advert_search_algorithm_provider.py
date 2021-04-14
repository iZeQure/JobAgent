import re
from datetime import datetime

from program.modules.services.data_service import DataService
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider


class JobAdvertSearchAlgorithmProvider(SearchAlgorithmProvider):
    __not_found_text = "Ikke Fundet"
    __not_found_id = 0

    _title_filter_keys: []
    _email_filter_keys: []
    _phone_number_filter_keys: []
    _description_filter_keys: []
    _location_name_filter_keys: []
    _registration_date_name_filter_keys: []
    _deadline_date_name_filter_keys: []
    _category_filter_keys: []
    _specialization_filter_keys: []

    def __init__(self, data_service: DataService):
        super().__init__(data_service)

        self.load_filters()

    def load_filters(self):
        service = self.data_service

        self._title_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Title_Key"))
        self._email_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Email_Key"))
        self._phone_number_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Phone_Number_Key"))
        self._description_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Description_Key"))
        self._location_name_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Location_Key"))
        self._registration_date_name_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Registration_Date_Key"))
        self._deadline_date_name_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Deadline_Date_Key"))
        self._category_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Category_Key"))
        self._specialization_filter_keys = \
            self.get_keys_from_list(service.get_algorithm_keywords_by_key("Specialization_Key"))

    def find_title(self) -> str:
        for arg in self._title_filter_keys:
            title = self.soup.find(text=re.compile(arg, flags=re.IGNORECASE))

            if title is not None:
                return title

        return self.__not_found_text

    def find_email(self) -> str:
        for arg in self._email_filter_keys:
            mail = self.soup.select_one(arg)

            if mail is not None:
                return mail.get_text()

        return self.__not_found_text

    def find_phone_number(self) -> str:
        regex = "([0-9]{8,8})"

        for arg in self._phone_number_filter_keys:
            for result in self.soup.find_all(arg):
                text_from_result = result.get_text()

                if text_from_result is not None:
                    if re.match(pattern=regex, string=str(text_from_result)):
                        return text_from_result

        return self.__not_found_text

    def find_description(self) -> str:
        for arg in self._description_filter_keys:
            description = self.soup.select_one(arg)

            if description is not None:
                return description.get_text(' ', True)

        return self.__not_found_text

    def find_location(self) -> str:
        found_location_element = None
        elements = []

        for location_filter in self._location_name_filter_keys:
            elements.append(self.soup.find(text=re.compile(location_filter, flags=re.IGNORECASE)))

        for arg in elements:
            if arg is not None:
                found_location_element = arg

        if found_location_element is not None:
            actual_location = found_location_element.parent.nextSibling
            if actual_location is not None:
                return actual_location.get_text(separator=' ')

        return self.__not_found_text

    def find_registration_date(self) -> datetime:
        reg_datetime = None
        date_elements = []
        for date_filter in self._registration_date_name_filter_keys:
            date_elements.append(self.soup.find(text=re.compile(date_filter), flags=re.IGNORECASE))

        if date_elements is not None:
            for date in date_elements:
                if date is not None:
                    if date.has_attr('datetime'):
                        reg_datetime = date['datetime']

        if reg_datetime is not None:
            reg_datetime = datetime.strptime(reg_datetime, '%d.%m.%Y')
        else:
            reg_datetime = datetime.today()

        return reg_datetime

    def find_deadline_date(self) -> datetime:
        return datetime.today()
        # date_time_str = self.get_actual_result_from_element(
        #     self.get_date_elements(
        #         self._deadline_date_name_filter_keys
        #     )
        # )
        #
        # date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        # return date_time_obj

    def find_category(self) -> int:
        category_filter_keys = self._category_filter_keys
        categories = self.data_service.get_categories()
        category_id = 0
        result = ""

        for category_filter_key in category_filter_keys:
            result = self.soup.find(text=re.compile(category_filter_key, flags=re.IGNORECASE))
            if result is not None:
                result = result.lower()

        if result is not None:
            for i, category in enumerate(categories):
                if str(category[1]).lower() in result:
                    category_id = category[0]
                    break

            if category_id != 0:
                return category_id
        else:
            return self.__not_found_id

    def find_specialization(self) -> int:
        specialization_filter_keys = self._specialization_filter_keys
        specializations = self.data_service.get_specializations()
        specialization_id = 0

        for specialization_filter in specialization_filter_keys:
            result = self.soup.find(text=re.compile(specialization_filter, flags=re.IGNORECASE))
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

        return self.__not_found_id

    @staticmethod
    def get_keys_from_list(keys: []) -> []:
        temp_list = []
        for key in keys:
            temp_list.append(key[0])
        return temp_list
