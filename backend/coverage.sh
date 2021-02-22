#!/usr/bin/env bash

rm -r Tests/TestResults
rm -r coverage

dotnet test --collect="XPlat Code Coverage" --settings runsettings.xml

TRPATH=($(ls Tests/TestResults))

reportgenerator "-reports:Tests/TestResults/$TRPATH/coverage.cobertura.xml" "-targetdir:coverage" -reporttypes:Html