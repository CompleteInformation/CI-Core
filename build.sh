#!/bin/bash
cd CompleteInformation.Core && dotnet restore && dotnet fake build -t Release
