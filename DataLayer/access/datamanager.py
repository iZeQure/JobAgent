import DataLayer.database as db
import DataLayer.models as models


class DataManager:
    __database: db.DatabaseMSSQL

    def __init__(self, database):
        self.__database = database

    def get_initialization_information(self):
        c = self.__database.connect(database="ZombieCrawlerDB")

        c.execute('EXEC [GetInitializationInformation]')
        result = c.fetchall()
        c.close()
        del c
        return result

    def get_algorithm_keywords_by_key_value(self, key_value: str):
        context = self.__database.connect(database='ZombieCrawlerDB')
        sp_sql = f'EXEC [GetKeysByKeyValue] @key_value=?'
        return list(context.execute(sp_sql, key_value))

    def get_jobadvert_ids(self):
        context = self.__database.connect()
        sql = 'SELECT [VacantJobId] FROM [JobAdvert];'
        return list(context.execute(sql))

    def get_categories(self):
        context = self.__database.connect()
        sp_sql = f'EXEC [JA.spGetCategories]'
        return list(context.execute(sp_sql))

    def get_specializations(self):
        context = self.__database.connect()
        sp_sql = f'EXEC [JA.spGetSpecializations]'
        return list(context.execute(sp_sql))

    def get_companies(self):
        c = self.__database.connect()
        c.execute('EXEC [JA.spGetCompanies]')
        result = c.fetchall()
        c.close()
        del c
        return result

    def get_vacant_jobs(self):
        c = self.__database.connect(database='JobAgentDB_v2')
        c.execute('EXEC [dbo].[JA.spGetVacantJobs]')
        output = []
        for result in c.fetchall():
            output.append([result[0], result[1], result[2], 'None'])
        c.close()
        del c
        return output

    def get_vacant_job_id_from_jobadvert(self):
        context = self.__database.connect(database='JobAgentDB_v2')
        sql = 'SELECT [VacantJobId] FROM [JobAdvert]'
        return list(context.execute(sql))

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
