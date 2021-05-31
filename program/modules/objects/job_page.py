from program.modules.handlers.error_handler import ErrorHandler
from program.modules.handlers.property_handler import PropertyHandler
from program.modules.objects.base_job_entity import BaseJobEntity
from program.modules.objects.page import Page


class JobPage(BaseJobEntity, Page):
    """
    Handling the content in a Job Page.
    """
    def __init__(self, entity_id: int, company_id: int, urls: [], page_source: str):
        """
        Instantiates an entity of Job Page.
        Args:
            entity_id: The ID of the entity.
            company_id: The ID of the Company entity.
            urls: A list of URLs for the Company Page.
            page_source: A string representing the page source.
        """
        super(JobPage, self).__init__(entity_id)

        self._company_id = company_id
        self._urls = urls
        self.set_page_source(page_source)

    @property
    def get_company_id(self):
        """
        Get the entity ID for Company.
        Returns:
            int: An ID describing the unique entity.

        Raises:
            ValueError: If the ID is not type of int.
        """
        return self._company_id

    @property
    def get_urls(self):
        """
        Get list of job pages.
        Returns:
            []: Represent the list of job pages.

        """
        return self._urls
