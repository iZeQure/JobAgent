from program.modules.handlers.error_handler import ErrorHandler
from program.modules.handlers.property_handler import PropertyHandler
from program.modules.objects.base_job_entity import BaseJobEntity
from program.modules.objects.page import Page


class VacantJob(BaseJobEntity, Page):
    """
    Handles the base logic of Vacant Job.
    """
    def __init__(self, vacant_job_id: int, url: str, company_id: int, page_source: str):
        """
        Instantiates a Vacant Job entity.
        Args:
            vacant_job_id: Represents the unique ID for the entity.
            url: The specific link of the vacant job.
            company_id: Represents the unique ID for company entity.
            page_source: Represents the page source of the link.
        """
        super(VacantJob, self).__init__(vacant_job_id)

        self._url = url
        self._company_id = company_id
        self.set_page_source(page_source)

    @property
    def get_url(self) -> str:
        """
        Get the url of the vacant job.
        Returns:
            str: A string representing the url for the vacant job.

        Raises:
            ValueError: If the object is not type of str.

        """
        return self._url

    @property
    def get_company_id(self) -> int:
        """
        Get the unique ID of the company entity.
        Returns:
            int: An integer representing the value of the entity.

        Raises:
            ValueError: If the object is not type of int.

        """
        return self._company_id
