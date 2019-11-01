pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/core/sdk:3.0'
        }
    }
    environment {
        CI = 'true'
    }
    stages {
        stage('build') {
            steps {
                sh 'dotnet restore'
            }
        }
        stage('approval') {
            timeout(time: 30, unit: 'DAYS') {
                input message: "Start second rollout ?"
            }
        }
        stage('test') {
            steps {
                sh 'dotnet test'
            }
        }
    }
}