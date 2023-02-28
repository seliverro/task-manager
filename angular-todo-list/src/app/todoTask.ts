export interface TodoTask {
    id: number;
    parentId: number;
    summary: string;
    description: string;
    created: string;
    dueDate: string;
    priority: number;
    status: number;
}