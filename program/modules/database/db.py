import pyodbc
import logging as log


class DatabaseMSSQL(object):
    """
    Represents a class handling the connection to an MSSQL Database.
    """
    __database_obj = pyodbc
    __connection_string: str

    def __init__(self, app_config: object):
        self.__build_config(app_config)

    def connect(self,
                auto_commit: bool = False,
                database_scheme: str = 'JobAgentDB_v2') -> pyodbc.Cursor:
        """
        Instantiates a new connection to the database.
        Returns:
            object: Returns a pyodbc cursor with the created connection.
        """
        cursor: pyodbc.Cursor
        conn: pyodbc.Connection

        try:
            if self.__connection_string == '':
                raise ValueError('Connection string was not fulfilled.')
            else:
                conn = self.__database_obj.connect(
                    self.__connection_string,
                    autocommit=auto_commit,
                    database=database_scheme)
                with conn:
                    cursor = conn.cursor()
                    return cursor
        except ValueError as er:
            log.info(er)
        except Exception as ex:
            log.exception(ex)

    def __build_config(self, app_config: object):
        """
        Builds the configuration for the database connection.
        Args:
            app_config: Contains the configuration for the database connection properties.

        Returns: None.

        """
        database_config = app_config["DatabaseConnection"]

        if 'UID' in database_config and 'PWD' in database_config:
            self.__connection_string = f'DRIVER={database_config["Driver"]};' \
                                       f'SERVER={database_config["Server"]};' \
                                       f'UID={database_config["UID"]};' \
                                       f'PWD={database_config["PWD"]}' \
                                       f'TRUSTED_CONNECTION={database_config["Trusted_Connection"]}'
            return

        self.__connection_string = f'DRIVER={database_config["Driver"]};' \
                                   f'SERVER={database_config["Server"]};' \
                                   f'TRUSTED_CONNECTION={database_config["Trusted_Connection"]}'
