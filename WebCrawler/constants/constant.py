class Constant:
    _search_algorithm_not_found_text: str
    _search_algorithm_not_found_identifier: int

    def __init__(self):
        self._search_algorithm_not_found_text = 'Ikke fundet'
        self._search_algorithm_not_found_identifier = 0

    def get_notfound_text(self) -> str:
        """
        Returns not found text.
        """
        return self._search_algorithm_not_found_text

    def get_notfound_identifier(self) -> int:
        """
        Returns zero.
        """
        return self._search_algorithm_not_found_identifier
