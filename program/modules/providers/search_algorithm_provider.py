import logging as log
from bs4 import BeautifulSoup
from program.modules.services.data_service import DataService


class SearchAlgorithmProvider(object):
    """
        Super class of Search Algorithms, provides the most basic information for crawling the web.
    """
    __service: DataService
    __soup: BeautifulSoup

    def __init__(self, data_service: DataService):
        """
        Instantiates the Algorithm provider.
        Args:
            data_service: Represents a data service, to give proper methods to execute from.
        """
        self.__service = data_service

    def set_page_source(self, page_html: str):
        """
        Instantiates a BeautifulSoup object with the given HTML.
        Args:
            page_html: Page source of the URL provided.
        Returns: None.
        """
        self.__soup = BeautifulSoup(page_html, 'html.parser')

    @property
    def data_service(self):
        """
        Get the data service.
        Returns:
            DataService: A DataService object.

        """
        try:
            return self.__service
        except Exception as ex:
            log.error(f'No instance found of [service] in {ex.__class__}')

    @property
    def soup(self):
        """
        Get the soup.
        Returns:
            BeautifulSoup: An object containing the Beautiful Soup object.
        """
        try:
            return self.__soup
        except Exception as ex:
            log.error(f'No instance found of [soup] in {ex.__class__}')