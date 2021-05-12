from program.modules.objects.base_job_entity import BaseJobEntity


class JobPage(BaseJobEntity):
    __company_id: int
    __url: []

    def __init__(self, entity_id: int, company_id: int, url: [], page_source: str):
        super().__init__(entity_id, page_source)

        self.__company_id = company_id
        self.__url = url

    @property
    def company_id(self):
        return self.__company_id

    @property
    def url(self):
        return self.__url
