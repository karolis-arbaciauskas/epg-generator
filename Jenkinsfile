pipeline {
    agent { docker { image 'mcr.microsoft.com/dotnet/core/sdk:3.0' } }
    stages {
        stage('build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet test'
            }
        }
    }
}