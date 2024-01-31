import { User } from "./user";

export interface IProfile {
    username: string;
    displayName: string;
    image?: string;
    bio?: string;
    photos?: Phtoto[];
}

export class Profile implements IProfile {
    constructor(user: User) {
        this.username = user.userName;
        this.displayName = user.displayName;
        this.image = user.image;
    }

    username: string;
    displayName: string;
    image?: string;
    bio?: string;
    photos?: Phtoto[];
}

export interface Phtoto {
    id: string;
    url: string;
    isMain: boolean;
}