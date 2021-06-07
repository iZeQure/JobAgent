from datetime import datetime

from program.modules.objects.address import Address
from program.modules.objects.base_job_entity import BaseJobEntity


class JobAdvert(BaseJobEntity):
    """
    Handles the base logic of a Job Advert.
    """
    def __init__(self, vacant_job_id: int, category_id: int, specialization_id: int, title: str, summary: str,
                 description: str, email: str, phone_number: str, registered_date_time: datetime,
                 application_deadline_date_time: datetime, address: Address):
        """
        Instantiates an entity of Job Advert.
        Args:
            vacant_job_id: The unique ID of the entity.
            category_id: Represents the ID specifying the category of the job advert.
            specialization_id: Represents the ID specifying the specialization of the job advert.
            title: Describes the title of the job advert.
            summary: Represents a short summary of the description.
            description: A full length text of the job advert.
            email: The unique email of the job advert.
            phone_number: The unique phone number of the job advert.
            registered_date_time: The datetime of which the job advert was created.
            application_deadline_date_time: The datetime of which the deadline is at.
            address: An address of the job advert.
        """
        super(JobAdvert, self).__init__(vacant_job_id)

        self._category_id = category_id
        self._specialization_id = specialization_id
        self._title = title
        self._summary = summary
        self._description = description
        self._email = email
        self._phone_number = phone_number
        self._registered_date_time = registered_date_time
        self._application_deadline_date_time = application_deadline_date_time
        self._address = address

    @property
    def get_category_id(self) -> int:
        """
        Get the category id.
        Returns:
            int: an integer representing the id of the category.

        Raises:
            ValueError: if the object is not type of int.

        """
        return self._category_id

    @property
    def get_specialization_id(self) -> int:
        """
        Get the specialization id.
        Returns:
            int: an integer representing the id of the specialization.

        Raises:
            ValueError: if the object is not type of int.

        """
        return self._specialization_id

    @property
    def get_title(self) -> str:
        """
        Get the title.
        Returns:
            str: a string representing the title of the job advert.

        Raises:
            ValueError: if the object is not type of str.

        """
        return self._title

    @property
    def get_summary(self) -> str:
        """
        Get the summary.
        Returns:
            str: a string representing the summary of the job advert.

        Raises:
            ValueError: if the object is not type of str.

        """
        return self._summary

    @property
    def get_(self) -> str:
        """
        Get the description.
        Returns:
            str: a string representing the full text of the job advert..

        Raises:
            ValueError: if the object is not type of str.

        """
        return self._description

    @property
    def get_email(self) -> str:
        """
        Get the email.
        Returns:
            str: a string representing the email of the job advert.

        Raises:
            ValueError: if the object is not type of str.

        """
        return self._email

    @property
    def get_phone_number(self) -> str:
        """
        Get the phone number.
        Returns:
            str: a string representing the numeric phone number.

        Raises:
            ValueError: if the object is not type of int.

        """
        return self._phone_number

    @property
    def get_registration_datetime(self) -> datetime:
        """
        Get the registration datetime.
        Returns:
            datetime: a datetime representing the time the job advert was registered.

        Raises:
            ValueError: if the object is not type of datetime.

        """
        return self._registered_date_time

    @property
    def get_application_deadline_datetime(self) -> datetime:
        """
        Get the deadline datetime.
        Returns:
            datetime: a datetime representing the time the job advert has deadline for.

        Raises:
            ValueError: if the object is not type of datetime.

        """
        return self._application_deadline_date_time

    @property
    def get_address(self) -> Address:
        """
        Get the Address.
        Returns:
            Address: the object address, that describes the location of the job advert.

        Raises:
            ValueError: if the object is not type of Address.

        """
        return self._address
