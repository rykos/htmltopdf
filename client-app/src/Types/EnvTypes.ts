declare global {
    namespace NodeJS {
        interface ProcessEnv {
            REACT_APP_API_LINK: string
        }
    }
}

export { }