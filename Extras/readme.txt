Deploy.config.xml - zawiera ścieżki do projektów, które mają zostac wysłane na serwer.
	Jeśli typ scieżki to 'bpmproj' to proejkty bibliotek nie są wysyłane. Jeśli typ to 'solution', to nie ma takiego ograniczenia.
	
deploy.exe.config - zawiera ścieżkę dla parametru *DeploymentServiceConfig*, dane logowania do Deployment Service oraz instrukcje przesyłania 
					informacji o działaniu programu informacji do pliku.
					
FlakeDeploymentServiceConfig.xml - zawiera dane o Deployment Service, czyli serwera, na którym stoi dane środowisko (Engine)