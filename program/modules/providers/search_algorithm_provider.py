import logging as log
from bs4 import BeautifulSoup
from program.modules.managers.database_manager import DatabaseManager


class SearchAlgorithmProvider:
    """
        Super class of Search Algorithms, provides the most basic information for crawling the web.
    """
    __manager: DatabaseManager
    __soup: BeautifulSoup

    def __init__(self, manager: DatabaseManager) -> None:
        """
        Instantiates the Algorithm provider.
        Args:
            manager: Represents a data manager, to give proper methods to execute from.
        """
        self.__manager = manager

    def get_data(self, data_object: object):
        log.info('You got fooled.')

    def set_page_source(self, page_html: str) -> None:
        """
        Instantiates a BeautifulSoup object with the given HTML.
        Args:
            page_html: Page source of the URL provided.
        Returns: None.
        """
        self.__soup = BeautifulSoup(page_html, 'html.parser')

    @property
    def manager(self) -> DatabaseManager:
        """
        Get the data manager.
        Returns:
            DatabaseManager: A DatabaseManager object.

        """
        try:
            return self.__manager
        except Exception as ex:
            log.error(f'No instance found of [service] in {ex.__class__}')

    @property
    def soup(self) -> BeautifulSoup:
        """
        Get the soup.
        Returns:
            BeautifulSoup: An object containing the Beautiful Soup object.
        """
        try:
            return self.__soup
        except Exception as ex:
            log.error(f'No instance found of [soup] in {ex.__class__}')