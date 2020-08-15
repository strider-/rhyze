import 'bootstrap/dist/css/bootstrap.min.css'
import '../styles/global.css'
import Header from '../components/header';
import { Container } from 'react-bootstrap';

function MyApp({ Component, pageProps, router }) {
    if(router.pathname.startsWith('/login')){
        return <Component {...pageProps} />
    } else {
        return <>
            <Header />
            <main className='main'>
                <Container>
                    <Component {...pageProps} />
                </Container>
            </main>
        </>;      
    }
}

export default MyApp
