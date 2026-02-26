
@echo off
echo Ejecutando cURL de Postman...
echo.
curl --location --request GET 'https://gmterpv8-50.gmtransport.co/api/contabilidad/consultarprepolizas' --header 'Aplicacion: 1' --header 'Authorization: Basic U0lTVEVNQVM6QWlqRXpyUWhydkNVWDNOPQ==' --header 'Content-Type: application/json' --header 'rfc: LMA040227500' --data '{\"FechaInicial\":\"20241126\",\"FechaFinal\":\"20251126\",\"Pagina\":\"2\",\"Proceso\":\"Pasivos\"}'
echo.
echo -----------------------------------------
echo.
echo Ahora ejecutando con RestSharp...
echo.
pause
