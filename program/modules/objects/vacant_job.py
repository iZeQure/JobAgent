from program.modules.objects.base_job_entity import BaseJobEntity


class VacantJob(BaseJobEntity):
    __link: str
    __company_id: int

    def __init__(self,
                 vacant_job_id: int,
                 link: str,
                 company_id: int,
                 html_page_source: str):

        super().__init__(vacant_job_id, html_page_source)
        self.__link = link
        self.__company_id = company_id

    @property
    def link(self) -> str:
        return self.__link

    @property
    def company_id(self) -> int:
        return self.__company_id
