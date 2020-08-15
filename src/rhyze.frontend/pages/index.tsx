import { AuthProps, privateRoute } from "../components/private_route";
import Albums from '../components/albums';

function Home({ auth }: AuthProps) {
    return <Albums auth={auth} />;
}

export default privateRoute(Home);