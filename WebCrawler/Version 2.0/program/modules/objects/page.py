class Page(object):
    """
    Handles the base of a page object.
    """
    def __init__(self):
        """
        Instantiates a Page entity.
        """
        self._page_source = ''

    @property
    def get_page_source(self):
        """
        Get the page source of the object.
        Returns:
            str: A string representing the page source.

        Raises:
            ValueError: If the defined object is not type of str.
        """
        return self._page_source

    def set_page_source(self, page_source: str):
        """
        Set the page source of the object.
        Args:
            page_source: A string representing the page source to set.

        Raises:
            ValueError: If the defined parameter is not type of str.
        """
        self._page_source = page_source
