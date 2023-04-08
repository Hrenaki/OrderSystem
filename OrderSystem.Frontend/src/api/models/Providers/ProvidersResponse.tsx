
export interface Provider {
    id: number,
    name: string
}

export default interface ProvidersResponse {
    providers: Provider[],
    error: string
}