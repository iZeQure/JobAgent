import WebCrawler.DataLayer.database as db
import WebCrawler.DataLayer.models as models


class DataManager:
    __database: db.DatabaseMSSQL

    def __init__(self, database):
        self.__database = database

    def get_initialization_information(self) -> []:
        context = self.__database.connect(database="ZombieCrawlerDB")
        sp_sql = 'EXEC [GetInitializationInformation]'

        context.execute(sp_sql)
        result = context.fetchall()
        context.close()
        del context

        return result

    def get_algorithm_keywords_by_key_value(self, key_value: str) -> []:
        context = self.__database.connect(database='ZombieCrawlerDB')
        sp_sql = f'EXEC [GetKeysByKeyValue] @key_value=?'

        context.execute(sp_sql, key_value)
        result = context.fetchall()
        context.close()
        del context

        return result

    def get_jobadvert_ids(self) -> []:
        context = self.__database.connect()
        sql = 'SELECT [VacantJobId] FROM [JobAdvert];'

        context.execute(sql)
        result = context.fetchall()
        context.close()
        del context

        return result

    def get_categories(self) -> []:
        context = self.__database.connect()
        sp_sql = f'EXEC [JA.spGetCategories]'

        context.execute(sp_sql)
        result = context.fetchall()
        context.close()
        del context

        return result

    def get_specializations(self) -> []:
        context = self.__database.connect()
        sp_sql = f'EXEC [JA.spGetSpecializations]'

        context.execute(sp_sql)
        result = context.fetchall()
        context.close()
        del context

        return result

    def get_companies(self) -> []:
        context = self.__database.connect()
        context.execute('EXEC [JA.spGetCompanies]')
        result = context.fetchall()
        context.close()
        del context
        return result

    def get_vacant_jobs(self) -> []:
        context = self.__database.connect(database='JobAgentDB_v2')
        context.execute('EXEC [dbo].[JA.spGetVacantJobs]')

        result = context.fetchall()
        context.close()
        del context

        return result

    def get_existing_job_adverts(self) -> []:
        context = self.__database.connect(database='JobAgentDB_v2')
        sql = 'SELECT [VacantJobId] FROM [JobAdvert]'

        context.execute(sql)
        result = context.fetchall()
        context.close()
        del context

        return result

    def create_jobadvert(self, obj: models.JobAdvert):
        context = self.__database.connect(True)
        sp_sql = """EXEC [JA.spCreateJobAdvert]
        @vacantJobId=?, 
        @categoryId=?, 
        @specializationId=?,
        @jobAdvertTitle=?, 
        @jobAdvertSummary=?,
        @jobAdvertDescription=?, 
        @jobAdvertEmail=?, 
        @jobAdvertPhoneNr=?,
        @jobAdvertRegistrationDateTime=?, 
        @jobAdvertApplicationDeadlineDateTime=?;
        """
        params = (
            obj.id,
            obj.category_id,
            obj.specialization_id,
            obj.title,
            obj.summary,
            obj.description,
            obj.email,
            obj.phone_number,
            obj.registration_datetime,
            obj.application_deadline_datetime
        )

        context.execute(sp_sql, params)

    def create_address(self, obj: models.Address):
        context = self.__database.connect(True)
        sp_sql = """EXEC [JA.spCreateAddress]
        @jobAdvertVacantJobId=?,
        @streetAddress=?,
        @city=?,
        @postalCode=?;
        """
        params = (
            obj.id,
            obj.street_address,
            obj.city,
            obj.country,
            obj.postal_code
        )

        context.execute(sp_sql, params)

    def create_vacant_job(self, obj: models.VacantJob):
        context = self.__database.connect(True)
        sp_sql = """EXEC [JA.spCreateVacantJob]
        @vacantJobLink=?,
        @companyId=?;    
        """
        params = (
            obj.link,
            obj.company_id
        )

        context.execute(sp_sql, params)

    def update_jobadvert(self, jobadvert):
        context = self.__database.connect(True)
        sp_sql = sp_sql = """EXEC [JA.spUpdateJobAdvert]
        @vacantJobId=?, 
        @categoryId=?, 
        @specializationId=?,
        @jobAdvertTitle=?, 
        @jobAdvertSummary=?,
        @jobAdvertDescription=?, 
        @jobAdvertEmail=?, 
        @jobAdvertPhoneNr=?,
        @jobAdvertRegistrationDateTime=?, 
        @jobAdvertApplicationDeadlineDateTime=?;
        """
        params = (
            jobadvert.id,
            jobadvert.category_id,
            jobadvert.specialization_id,
            jobadvert.title,
            jobadvert.summary,
            jobadvert.description,
            jobadvert.email,
            jobadvert.phone_number,
            jobadvert.registration_datetime,
            jobadvert.application_deadline_datetime
        )

        context.execute(sp_sql, params)
