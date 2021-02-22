import * as React from "react"
import UserHome from "@/components/site/users/UserHome"
import { ApplicationUserModel } from "@/tools/definitions/users"

type HomeProps = {
	user: ApplicationUserModel
}

const Home: React.FunctionComponent<HomeProps> = ({ user }: HomeProps): JSX.Element => <UserHome identifier={`${user?.userName}'s`}/>

export default Home