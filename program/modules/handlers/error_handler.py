class ErrorHandler(object):
    @staticmethod
    def raise_value_error(data_type, property_type, property_name):
        raise ValueError(f'{property_name} was not type of: {data_type}, given type was: {type(property_type)}.')
