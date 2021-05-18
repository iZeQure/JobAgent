

class BaseJobEntity(object):
    __id: int
    __html_page_source: str

    def __init__(self,
                 entity_id: int,
                 html_page_source: str):
        self.__id = entity_id
        self.__html_page_source = html_page_source

    @property
    def id(self):
        return self.__id

    @property
    def html_page_source(self):
        return self.__html_page_source

    def set_page_source(self, page_source: str):
        self.__html_page_source = page_source
