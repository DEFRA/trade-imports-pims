module.exports = {
    preset: 'ts-jest',
    testEnvironment: 'node',
    moduleDirectories: ['node_modules', 'src'], // Allow Jest to resolve imports from `src`
    moduleNameMapper: {
        '^src/(.*)$': '<rootDir>/src/$1', // Ensure Jest can resolve `src/` imports correctly
    },
    transform: {
        '^.+\\.tsx?$': ['ts-jest', {
            tsconfig: 'tsconfig.jest.json',
        }],
    },
    modulePathIgnorePatterns: [
        '/out/tests/' // Ignore the compiled spec files in `out` directory
    ],
    reporters: [
        'default',
        ['jest-junit', {
            outputDirectory: 'test-results',
            outputName: 'junit.xml',
        }],
    ],
    collectCoverage: true,
    collectCoverageFrom: [
        'src/**/*.ts',
        '!typings/**/*.d.ts', // Exclude type declaration files
    ],
    coverageDirectory: 'coverage', // Directory where coverage reports will be saved
};
