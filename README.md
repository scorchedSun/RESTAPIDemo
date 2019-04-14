# RESTAPIDemo
A small demo project of an ASP.NET Core MVC RESTful API managing persons

# REST endpoints

GET api/persons - Returns all persons

GET api/persons/{id} - Returns a person by ID

GET api/persons/color/{colour} - Returns all persons having the same favourite colour

# Example result

    GET api/persons/{id}
    {  
       "id":1,  
       "name":"Test",  
       "lastname":"Tester",  
       "zipcode":"00000",  
       "city":"Test City",  
       "color":"yellow"  
    }
