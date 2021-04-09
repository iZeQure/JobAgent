import datetime
from .address import Address


class JobAdvert:
    __vacant_job_id = int
    __category_id = int
    __specialization_id = int
    __title = str
    __summary = str
    __description = str
    __email = str
    __phone_number = str
    __registered_date_time = datetime
    __application_deadline_date_time = datetime
    __address = Address

    def __init__(self, vacant_job_id: int, category_id: int, specialization_id: int,
                 title: str, summary: str, description: str, email: str, phone_number: str,
                 registration_datetime: datetime, application_deadline_datetime: datetime,
                 address: Address):
        """
        Initializes a new job advert class.
        Args:
            vacant_job_id: A unique id of the object entity.
            category_id: A unique id of the category entity.
            specialization_id: A unique id of the specialization entity.
            title: Title describing the job advert.
            summary: A short summary text of the job advert.
            description: A long description text of the job advert.
            email:
            phone_number:
            registration_datetime:
            application_deadline_datetime:
            address: Specifies the job advert Address.
        """
        self.__vacant_job_id = vacant_job_id
        self.__category_id = category_id
        self.__specialization_id = specialization_id
        self.__title = title
        self.__summary = summary
        self.__description = description
        self.__email = email
        self.__phone_number = phone_number
        self.__registered_date_time = registration_datetime
        self.__application_deadline_date_time = application_deadline_datetime
        self.__address = address

    @property
    def id(self) -> int:
        return self.__vacant_job_id

    @property
    def category_id(self) -> int:
        return self.__category_id

    @property
    def specialization_id(self) -> int:
        return self.__specialization_id

    @property
    def title(self) -> str:
        return self.__title

    @property
    def summary(self) -> str:
        return self.__summary

    @property
    def description(self) -> str:
        return self.__description

    @property
    def email(self) -> str:
        return self.__email

    @property
    def phone_number(self) -> str:
        return self.__phone_number

    @property
    def registration_datetime(self) -> datetime:
        return self.__registered_date_time

    @property
    def application_deadline_datetime(self) -> datetime:
        return self.__application_deadline_date_time

    @property
    def address(self) -> Address:
        return self.__address
