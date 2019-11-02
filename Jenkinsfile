pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/core/runtime:3.0'
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
        stage('test') {
            steps {
                sh 'dotnet test'
            }
        }
        stage('approval') {
            steps {
                timeout(time: 30, unit: 'DAYS') {
                    input message: "Pre-deploy check"
                }
            }
        }
    }
}