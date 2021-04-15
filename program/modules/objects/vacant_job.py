

class VacantJob(object):
    __id: int
    __link: str
    __company_id: int
    __html_page_source: str

    def __init__(self,
                 vacant_job_id: int,
                 link: str,
                 company_id: int,
                 html_page_source: str):
        self.__id = vacant_job_id
        self.__link = link
        self.__company_id = company_id
        self.__html_page_source = html_page_source

    @property
    def id(self) -> int:
        return self.__id

    @property
    def link(self) -> str:
        return self.__link

    @property
    def company_id(self) -> int:
        return self.__company_id

    @property
    def html_page_source(self) -> str:
        return self.__html_page_source
